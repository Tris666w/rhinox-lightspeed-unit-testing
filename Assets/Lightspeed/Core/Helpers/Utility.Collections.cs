﻿using System;
using System.Reflection;
using Rhinox.Lightspeed.Reflection;

namespace Rhinox.Lightspeed
{
    public partial class Utility
    {
        private static MethodInfo _arrayResizeStaticMethod;
        private static object[] _resizeArrayParameters;
        
        public static object ResizeArrayGeneric(object array, int newSize)
        {
            var type = array.GetType();
            if (!type.IsArray)
                throw new ArgumentException($"array was of type {type.GetNiceName()}, expected ArrayType");
            
            var elemType = type.GetElementType();
            if (_arrayResizeStaticMethod == null)
                _arrayResizeStaticMethod = typeof(Array).GetMethod("Resize", BindingFlags.Static | BindingFlags.Public);
            
            var properResizeMethod = _arrayResizeStaticMethod.MakeGenericMethod(elemType);
            if (_resizeArrayParameters == null)
                _resizeArrayParameters = new object[2];
            _resizeArrayParameters[0] = array;
            _resizeArrayParameters[1] = newSize;
            
            properResizeMethod.Invoke(null, _resizeArrayParameters);
            array = _resizeArrayParameters[0];
            return array;
        }
    }
}