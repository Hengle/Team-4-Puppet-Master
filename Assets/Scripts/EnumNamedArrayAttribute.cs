using UnityEngine;
public class EnumNamedArrayAttribute : PropertyAttribute
{
    /// <summary>
    /// Array containing the names of each element in the enum
    /// </summary>
    public string[] names;
    /// <summary>
    /// Constructor for the property
    /// </summary>
    /// <param name="names_enum_type">The typeof for the enum this array is based on</param>
    public EnumNamedArrayAttribute(System.Type names_enum_type)
    {
        //store the names of the enum in the names array
        this.names = System.Enum.GetNames(names_enum_type);
    }
}