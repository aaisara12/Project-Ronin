using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Obselete
{
    /// <summary>
    /// Hash table wrapper that represents mapping from string to state type.
    /// Written here so different characters can use different mappings (potentially mapping same string to different state vice versa)
    /// </summary>
    public interface IStateTable
    {
        ICharacterState GetState(string stateName);
    }
}