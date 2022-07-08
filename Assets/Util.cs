using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ID {
    public string name;
    public string displayName;
}

public abstract class SingletonComponent<T> : MonoBehaviour
where T : Component
{
    private static T My;
    public static T my
    {
        get {
            if(My == null) {
                var objs = FindObjectsOfType<T>() as T[];
                if(objs.Length > 0) {
                    My = objs[0];
                }
                if(objs.Length > 1) {
                    throw new System.Exception($"There are multiple instances of {typeof(T).Name}!");
                }
                if(My == null) {
                    GameObject obj = new GameObject();
                    obj.name = string.Format("_{0}", typeof(T).Name);
                    My = obj.AddComponent<T>();
                }
            }
            return My;
        }
    }
}

public static class Util
{
    public static GameObject[] FindAll(string name) {
        List<GameObject> found = new List<GameObject>();
        foreach(var item in GameObject.FindObjectsOfType<Transform>()) {
            if(item.gameObject.name == name) {
                found.Add(item.gameObject);
            }
        }
        return found.ToArray();
    }

    public static GameObject[] FindAllContaining(string name) {
        List<GameObject> found = new List<GameObject>();
        foreach(var item in GameObject.FindObjectsOfType<Transform>()) {
            if(item.gameObject.name.Contains(name)) {
                found.Add(item.gameObject);
            }
        }
        return found.ToArray();
    }

    public static GameObject GetFirstChild(string name, Transform childOf) {
        for(int i = 0; i < childOf.childCount; i++) {
            if(childOf.GetChild(i).name == name) {
                return childOf.GetChild(i).gameObject;
            }
        }
        return null;
    }

    public static Transform Player => GameObject.Find("Player").transform;

    public static IEnumerator MakeLarge2DArray<T>(T[] input, int height, int width, System.Action<T[,]> output) {
        T[,] tempOutput = new T[height, width];
        for(int h = 0; h < height; h++) {
            for(int w = 0; w < width; w++) {
                tempOutput[h,w] = input[h * width + w];
                Debug.Log($"Current pos: ({w},{h}) - height is {input[h * width + w]}");
            }
            yield return null;
        }
        output(tempOutput);
    }

    public static T[,] Make2DArray<T>(T[] input, int height, int width) {
        T[,] output = new T[height, width];
        for(int h = 0; h < height; h++) {
            for(int w = 0; w < width; w++) {
                output[h,w] = input[h * width + w];
            }
        }
        return output;
    }
    public static IEnumerator MakeLarge1DFrom2D<T>(T[,] input, int height, int width, System.Action<T[]> output) {
        T[] tempOutput = new T[height * width];
        for(int h = 0; h < height; h++) {
            for(int w = 0; w < width; w++) {
                tempOutput[h * width + w] = input[h, w];
            }
            yield return null;
        }
        output(tempOutput);

    }
    public static T[] Make1DFrom2D<T>(T[,] input, int height, int width) {
        T[] output = new T[height * width];
        for(int h = 0; h < height; h++) {
            for(int w = 0; w < width; w++) {
                output[h * width + w] = input[h, w];
            }
        }
        return output;
    }
}
