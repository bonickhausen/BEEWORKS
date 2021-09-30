using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnViewRigFPSTPS : PawnViewRigRotation
{
	[Header("Objects")]
	public GameObject ViewFirst;
	public GameObject ViewThird;
	[Header("State")]
	public ViewType View;

	private ViewType _lastViewType;

	public override bool ShouldShowSelfRenderer()
	{
		return View == ViewType.THIRD_PERSON;
	}

	protected override void Initialize()
	{
		base.Initialize();
		ChangeView();
	}

	public override void Tick()
	{
		base.Tick();

		CheckForChanges();

		void CheckForChanges()
		{
			if (View != _lastViewType)
			{
				ChangeView();
			}

			_lastViewType = View;
		}
	}

	private void ChangeView()
	{
		ViewFirst.SetActive(View == ViewType.FIRST_PERSON);
		ViewThird.SetActive(View == ViewType.THIRD_PERSON);
	}
}