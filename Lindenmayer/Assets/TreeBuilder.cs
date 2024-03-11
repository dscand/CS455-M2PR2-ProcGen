using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class TreeBuilder : MonoBehaviour
{
	private Coroutine builder;
	public float BranchDelayStart = 2.0f;
	private float BranchDelay = 0.0f;
	public GameObject BranchPrefab;
	public GameObject RootBranch;


	// Tree Classes
	public class Branch
	{
		public Branch(Vector3 position, Quaternion orientation, float length)
		{
			this.position = position;
			this.orientation = orientation;
			this.length = length;
		}

		public Vector3 position;
		public Quaternion orientation;
		public float length;

		public Vector3 end()
		{
			Vector3 pos = position;
			return pos + orientation * Vector3.up * length;
		}
		public Vector3 mid()
		{
			Vector3 pos = position;
			return pos + orientation * Vector3.up * length/2;
		}
	}

	public class Rule
	{
		public virtual bool matches(Branch branch) {return false;}
		public virtual Branch[] rhs(Branch branch) {return new Branch[0];}
	}

	public class Rule_ShortTerminate : Rule
	{
		public override bool matches(Branch branch) {return branch.length < 0.1;}
		public override Branch[] rhs(Branch branch) {return new Branch[0];}
	}
	public class Rule_DownTerminate : Rule
	{
		public override bool matches(Branch branch) {return Vector3.Dot(branch.orientation*Vector3.up, Vector3.down) > 0.5;}
		public override Branch[] rhs(Branch branch) {return new Branch[0];}
	}
	public class Rule_EqualSplit : Rule
	{
		public override bool matches(Branch branch) {return true;}
		public override Branch[] rhs(Branch branch)
		{
			Branch a = new(
				branch.end(),
				branch.orientation * Quaternion.Euler(0,0,-45),
				branch.length * 2 / 3
			);
			
			Branch b = new(
				branch.end(),
				branch.orientation * Quaternion.Euler(0,0,45),
				branch.length * 2 / 3
			);

			return new Branch[] {a, b};
		}
	}


	void Start()
	{
		Rule[] rules = {
			new Rule_ShortTerminate(),
			new Rule_DownTerminate(),
			new Rule_EqualSplit(),
		};

		Branch rootBranch = new(
			transform.position,
			RootBranch.transform.rotation,
			RootBranch.GetComponent<Renderer>().bounds.size.y
		);

		StartBuild(rootBranch, rules);
	}

	public void StartBuild(Branch rootBranch, Rule[] rules)
	{
		builder = StartCoroutine(BuildTree(rootBranch, rules));
	}
	public void StopBuild()
	{
		StopCoroutine(builder);
	}

	public IEnumerator BuildTree(Branch root, Rule[] rules)
	{
		//BranchDelay = BranchDelayStart;

		//List<Branch> tree = new List<Branch>() {root};
		Queue<Branch> active = new Queue<Branch>();
		active.Enqueue(root);

		while (active.Count > 0) {
			Branch current = active.Dequeue();
			//tree.Add(current);

			// Find and execute the first matching rule.
			List<Branch> newBranches = new List<Branch>();
			foreach (Rule rule in rules)
			{
				if (rule.matches(current))
				{
					// Add the new branches.
					Branch[] results = rule.rhs(current);
					foreach (Branch result in results)
					{
						active.Enqueue(result);
						newBranches.Add(result);
					}
					break;
				}

			}
			yield return RenderBranches(RootBranch, newBranches.ToArray()); //current.gameObject
			//BranchDelay *= 0.9f;
		}
		yield break;
	}
	public IEnumerator RenderBranches(GameObject parent, Branch[] branches)
	{
		foreach (Branch branch in branches)
		{
			GameObject newBranch = Instantiate(BranchPrefab, branch.mid(), branch.orientation, transform);
			newBranch.transform.localScale = new Vector3(newBranch.transform.localScale.x, branch.length, newBranch.transform.localScale.z);
			yield return new WaitForSeconds(BranchDelay);
		}
		yield break;
	}


	private Branch[] lSystem(Branch root, Rule[] rules)
	{
		List<Branch> tree = new List<Branch>() {root};
		Queue<Branch> active = new Queue<Branch>();
		active.Enqueue(root);

		while (active.Count > 0) {
			Branch current = active.Dequeue();
			tree.Add(current);

			// Find and execute the first matching rule.
			foreach (Rule rule in rules)
			{
				if (rule.matches(current))
				{
					// Add the new branches.
					Branch[] results = rule.rhs(current);
					foreach (Branch result in results)
					{
						active.Enqueue(result);
					}
					break;
				}
			}
		}

		return tree.ToArray();
	}
}
