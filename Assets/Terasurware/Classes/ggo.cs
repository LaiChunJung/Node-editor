using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ggo : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public bool ID;
		public string string_data;
		public double int_data;
		public double double_data;
		public bool bool_data;
		public bool math_1;
		public bool math_2;
		public double[] array;
	}
}