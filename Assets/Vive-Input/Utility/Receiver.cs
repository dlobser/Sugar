﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FRL.IO {

  /// <summary>
  /// Generic receiver object for global IO.
  /// <remarks>
  /// Attaching this component to a GameObject allows for it to receive
  /// global inputs from Holojam.IO input systems.
  /// </remarks>
  /// </summary>
  /// <seealso cref="UnityEngine.MonoBehaviour" />
  public sealed class Receiver : MonoBehaviour {

    /// <summary>
    /// Optional paired module for instance.
    /// <remarks>
    /// If not null, this instance will only receive input from this module.
    /// </remarks>
    /// </summary>
    public BaseInputModule module;

    private static List<Receiver> instanceCache = new List<Receiver>();
    private static System.Object instanceLock = new System.Object();


    private void Awake() {
      BaseInputModule mod = this.GetComponent<BaseInputModule>();
      if (module == null && mod != null) {
        module = mod;
      }
    }

    /// <summary>
    /// Gets the current instances of GlobalReceiver.
    /// </summary>
    /// <value>
    /// Returns a copy of the instances.
    /// </value>
    public static List<Receiver> instances {
      get {
        lock (instanceLock) {
          return instanceCache;
        }
      }
    }

    /// <summary>
    /// Returns a copied list of instances of GlobalReceiver.
    /// </summary>
    /// <returns></returns>
    public static List<Receiver> GetCopyOfInstances() {
      lock (instanceLock) {
        return new List<Receiver>(instanceCache);
      }
    }

    /// <summary>
    /// Adds this instance to the list of instances, when it is enabled.
    /// </summary>
    private void OnEnable() {
      lock (instanceLock) {
        instanceCache.Add(this);
      }
    }

    /// <summary>
    /// Removes this instance from the list of instances, when it is disabled.
    /// </summary>
    private void OnDisable() {
      lock (instanceLock) {
        instanceCache.Remove(this);
      }
    }
  }
}

