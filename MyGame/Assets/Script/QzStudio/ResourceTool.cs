using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourceTool  {

    public static GameObject GetResFromLocal(string fileName)
    {

        GameObject temp = Resources.Load<GameObject>(fileName);

        return temp;
    }
}
