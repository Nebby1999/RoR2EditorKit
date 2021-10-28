using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoR2EditorKit.Core
{
    public static class ErrorShorthands
    {
        public static NullReferenceException ThrowNullAssetName(string fieldName)
        {
            return new NullReferenceException($"Field {fieldName} cannot be Empty or Null");
        }

        public static NullReferenceException ThrowNullTokenPrefix()
        {
            return new NullReferenceException($"Your TokenPrefix in the RoR2EditorKit settings is Empty or Null");
        }
    }
}
