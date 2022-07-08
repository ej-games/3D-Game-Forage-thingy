using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class Game : SingletonComponent<Game>
{
    public delegate void Tick();
    public static Tick fixedUpdateTick, halfTick, secTick, tenTick, fifteenTick, twentyTick, thirtyTick, minuteTick;
    // Start is called before the first frame update
    void Start()
    {
        fixedUpdateTick += Nothing;
        halfTick += Nothing;
        secTick += Nothing;
        tenTick += Nothing;
        fifteenTick += Nothing;
        twentyTick += Nothing;
        thirtyTick += Nothing;
        minuteTick += Nothing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Nothing() {}

    double time;
    private void FixedUpdate()
    {
        time += 0.02;
        fixedUpdateTick();
        if(time % 0.5 == 0) halfTick();
        if(time % 1 == 0) secTick();
        if(time % 10 == 0) tenTick();
        if(time % 15 == 0) fifteenTick();
        if(time % 20 == 0) twentyTick();
        if(time % 30 == 0) thirtyTick();
        if(time % 60 == 0) minuteTick();
    }
}
