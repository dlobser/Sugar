class AssignMaterialRecursive extends ScriptableWizard
{
    var theMaterial : Material;
   
    function OnWizardUpdate()
    {
        helpString = "Select Game Obects";
        isValid = (theMaterial != null);
    }
   
    function OnWizardCreate()
    {
       
        var gos = Selection.gameObjects;
   
        for (var go in gos)
        {
//            go.GetComponent.<Renderer>().material = theMaterial;
            assigner(go);
        }
    }
    
    function assigner(pew : GameObject){
    
//		Debug.Log(pew); 
		if(pew.GetComponent.<Renderer>()!=null){
			pew.GetComponent.<Renderer>().material = theMaterial;
		}
		if(pew.transform.childCount>0){
			for(var i = 0 ; i < pew.transform.childCount ; i++){
				assigner(pew.transform.GetChild(i).gameObject);
			}
		}
    }
   
    @MenuItem("ON/Assign Material Recursive", false, 4)
    static function assignMaterial()
    {
        ScriptableWizard.DisplayWizard(
            "Assign Material Recursive", AssignMaterialRecursive, "Assign");
    }
}