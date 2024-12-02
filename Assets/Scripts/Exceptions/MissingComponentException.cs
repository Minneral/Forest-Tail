public class MissingComponentException : System.Exception
{
    public MissingComponentException(string componentName, string gameObjectName = "undefined", string scriptName = "undefined", string addictionalInfo = "") :  base($"Component '{componentName}' is missing on the '{gameObjectName}' GameObject in script '{scriptName}'.\nInfo: {addictionalInfo}")
     { }
}
