using System;
using System.Xml;
using UnityEngine;

class BaseOsm
{
    /// <summary>
    /// Get an attribute's value from the collection using the given 'attrName'. 
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="attrName">Name of the attribute</param>
    /// <param name="attributes">Node's attribute collection</param>
    /// <returns>The value of the attribute converted to the required type</returns>
    protected T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
{
    // Check if the attribute exists
    if (attributes[attrName] == null)
    {
        Debug.LogError($"Attribute '{attrName}' not found in the collection.");
        return default; // Return default value for the type
    }

    string strValue = attributes[attrName].Value;

    // Print out the attribute name and value for debugging
    //Debug.Log($"Attribute '{attrName}': {strValue}");

    try
    {
        // If the attribute is latitude or longitude, handle it accordingly
        if (typeof(T) == typeof(float) && (attrName == "lat" || attrName == "lon"))
        {
            // Parse latitude and longitude attributes directly
            return (T)(object)float.Parse(strValue, System.Globalization.CultureInfo.InvariantCulture);
        }
        else
        {
            // Attempt to convert the string value to the required type
            return (T)Convert.ChangeType(strValue, typeof(T));
        }
    }
    catch (Exception ex)
    {
        Debug.LogError($"Error converting attribute '{attrName}' value '{strValue}' to type {typeof(T)}: {ex.Message}");
        return default; // Return default value for the type
    }
}

}
