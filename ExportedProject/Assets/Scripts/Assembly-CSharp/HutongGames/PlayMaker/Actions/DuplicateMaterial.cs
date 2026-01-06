namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("♥ Caligula ♥")]
	[Tooltip("Duplicates a material and renames it.")]
	public class DuplicateMaterial : FsmStateAction
	{
		[Tooltip("Materials new name")]
		[RequiredField]
		public FsmString material_name;

		[RequiredField]
		[Tooltip("Material to duplicate")]
		public FsmMaterial material;

		[RequiredField]
		[Tooltip("New material variable")]
		public FsmMaterial new_material;

		public override void OnEnter()
		{
			Finish();
		}

		private FsmMaterial doNew(FsmMaterial vanha)
		{
			return new FsmMaterial(vanha);
		}

		private void setName(FsmMaterial matsku, string name)
		{
			matsku.Name = name;
		}
	}
}
