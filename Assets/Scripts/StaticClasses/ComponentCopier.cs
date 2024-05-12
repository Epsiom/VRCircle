using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentCopier
{
    /// <summary>
    /// Copies the components from the source GameObject to the destination GameObject
    /// </summary>
    public static void Copy(GameObject source, GameObject destination)
    {
        Debug.Log(source.name + " -> " + destination.name);

        // Get all components attached to the source GameObject
        Component[] components = source.GetComponents<Component>();

        foreach (Component comp in components)
        {
            // Skip Transform component as it's automatically created
            if (comp is Transform)
                continue;

            // Copy the component to the destination GameObject
            var newComponent = destination.AddComponent(comp.GetType());
            UnityEditorInternal.ComponentUtility.CopyComponent(comp);
            UnityEditorInternal.ComponentUtility.PasteComponentAsNew(destination);
        }
    }
}
