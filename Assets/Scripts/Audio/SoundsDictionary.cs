﻿using System;
using UnityEditor;
using UnityEngine;

[Serializable] public class SoundsDictionary : SerializableDictionary<SoundType, AudioClip> {}

[CustomPropertyDrawer(typeof(SoundsDictionary))]
public class MySoundDictionaryDrawer: DictionaryDrawer<SoundType, AudioClip> { }