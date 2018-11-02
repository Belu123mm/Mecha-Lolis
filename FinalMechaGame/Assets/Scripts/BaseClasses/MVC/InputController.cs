using System;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    HashSet<Tuple<Func<string, bool>, string, Action>> ButtonActions;

    HashSet<Tuple<Func<string, bool>, string, Func<string, float>, Action<float>>> SingleAxisActions;
    HashSet<Tuple<Func<string, bool>, Func<string, float>, string, string, Action<float, float>>> DoubleAxisActions;

    HashSet<Tuple<Func<string, float>, string, Action<float>>> SingleAxisTrack;
    HashSet<Tuple<Func<string, float>, string, string, Action<float, float>>> DoubleAxisTrack;

    private void Awake()
    {
        ButtonActions = new HashSet<Tuple<Func<string, bool>, string, Action>>();

        SingleAxisActions = new HashSet<Tuple<Func<string, bool>, string, Func<string, float>, Action<float>>>();
        DoubleAxisActions = new HashSet<Tuple<Func<string, bool>, Func<string, float>, string, string, Action<float, float>>>();

        SingleAxisTrack = new HashSet<Tuple<Func<string, float>, string, Action<float>>>();
        DoubleAxisTrack = new HashSet<Tuple<Func<string, float>, string, string, Action<float, float>>>();
    }

    private void Update()
    {
        foreach (var BindedAction in ButtonActions)
        {
            //Obtengo la función y el identificador.
            var func = BindedAction.Item1;
            string buttonName = BindedAction.Item2;

            //Ejecuto la función bindeada.
            if (func(buttonName)) BindedAction.Item3();
        }

        foreach (var BindedAxis in SingleAxisActions)
        {
            //Obtengo la función y el identificador.
            var func = BindedAxis.Item1;
            string AxisName = BindedAxis.Item2;

            //Ejecuto la función.
            if (func(AxisName)) BindedAxis.Item4(BindedAxis.Item3(AxisName));
        }

        foreach (var DoubleAxes in DoubleAxisActions)
        {
            //Obtengo las funciones de Chequeo, trigger y Axis.
            var check = DoubleAxes.Item1;
            var execute = DoubleAxes.Item5;
            string AxisName1 = DoubleAxes.Item3;
            string AxisName2 = DoubleAxes.Item4;
            var getAxis = DoubleAxes.Item2;

            //Ejecuto la función.
            if (check(AxisName1) || check(AxisName2)) execute(getAxis(AxisName1), getAxis(AxisName2));
        }

        foreach (var input in SingleAxisTrack)
        {
            //Ejecuto la función.
            input.Item3(input.Item1(input.Item2));
        }

        foreach (var Axis in DoubleAxisTrack)
        {
            //Ejecuto la función.
            Axis.Item4(Axis.Item1(Axis.Item2), Axis.Item1(Axis.Item3));
        }
    }

    //Add to Simple Buttom Action
    /// <summary>
    /// Binds a Function to an Input event with an associated Identifier.
    /// The Identifier must be Setted in the Editor ProjectSettings > Input tab.
    /// </summary>
    /// <param name="Check">The Input Event that Checks if the button has been Pressed.</param>
    /// <param name="ButtonName">The Identifier of the Button.</param>
    /// <param name="Execute">The Function to be binded with the chosen Input Event.</param>
    public void Bind(Func<string,bool> Check, string ButtonName, Action Execute)
    {
        Tuple<Func<string, bool>, string, Action> toBind;
        toBind = Tuple.Create(Check, ButtonName, Execute);
        ButtonActions.Add(toBind);
    }

    //Add to Single Axis Actions.
    /// <summary>
    /// Binds a Function to an Input event with an associated Identifier.
    /// The Identifier must be Setted in the Editor ProjectSettings > Input tab.
    /// </summary>
    /// <param name="Check">The Input Event that Checks if the button has been Pressed.</param>
    /// <param name="AxisGetter">The Input Event that gets the identified axis value.</param>
    /// <param name="Axis">The Identifier associated to the Axis.</param>
    /// <param name="ExecuteFuntion">The Function to be binded with the chosen Input Event.</param>
    public void Bind(Func<string, bool> Check, Func<string, float> AxisGetter, string Axis, Action<float> ExecuteFuntion)
    {
        Tuple<Func<string, bool>, string, Func< string, float>, Action<float>> toBind;
        toBind = Tuple.Create(Check, Axis, AxisGetter, ExecuteFuntion);
        SingleAxisActions.Add(toBind);
    }
    //Add to Double Axis Actions.
    /// <summary>
    /// Binds a Function to an Input event with an associated Identifier.
    /// The Identifier must be Setted in the Editor ProjectSettings > Input tab.
    /// </summary>
    /// <param name="Check">The Input Event that Checks if the button has been Pressed.</param>
    /// <param name="AxisGetter">The Input Event that gets the identified axis value.</param>
    /// <param name="AxisA">The Identifier associated to the first Axis.</param>
    /// <param name="AxisB">The Identifier associated to the second Axis.</param>
    /// <param name="ExecuteFuntion">The Function to be binded with the chosen Input Event.</param>
    public void Bind(Func<string,bool> Check,Func<string,float> AxisGetter,string AxisA, string AxisB, Action<float,float> ExecuteFuntion)
    {
        Tuple<Func<string, bool>, Func<string, float>, string, string, Action<float, float>> toBind;

        toBind = Tuple.Create(Check, AxisGetter, AxisA, AxisB, ExecuteFuntion);
        DoubleAxisActions.Add(toBind);
    }

    //Add to Single Axis Track
    /// <summary>
    /// Binds a Function to an Input event with an associated Identifier.
    /// The Identifier must be Setted in the Editor ProjectSettings > Input tab.
    /// </summary>
    /// <param name="AxisGetter">The Input Event that gets the identified axis value.</param>
    /// <param name="AxisName">The Identifier associated to the Axis.</param>
    /// <param name="Execute">The Function to be binded with the chosen Input Event.</param>
    public void Bind(Func<string, float> AxisGetter, string AxisName, Action<float>Execute)
    {
        Tuple<Func<string, float>, string, Action<float>> toBind;
        toBind = Tuple.Create(AxisGetter, AxisName, Execute);
        SingleAxisTrack.Add(toBind);
    }
    //Add to Double Axis Track.
    /// <summary>
    /// Binds a Function to an Input event with an associated Identifier.
    /// The Identifier must be Setted in the Editor ProjectSettings > Input tab.
    /// </summary>
    /// <param name="AxisGetter">The Input Event that gets the identified axis value.</param>
    /// <param name="AxisA">The Identifier associated to the first Axis.</param>
    /// <param name="AxisB">The Identifier associated to the second Axis.</param>
    /// <param name="ExecuteFuntion">The Function to be binded with the chosen Input Event.</param>
    public void Bind(Func<string, float> AxisGetter, string AxisA,string AxisB, Action<float,float> ExecuteFuntion)
    {
        Tuple<Func<string, float>, string, string, Action<float, float>> toBind;
        toBind = Tuple.Create(AxisGetter,AxisA,AxisB,ExecuteFuntion);
        DoubleAxisTrack.Add(toBind);
    }
}
