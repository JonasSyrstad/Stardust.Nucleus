//
// callstackitem.cs
// This file is part of Stardust
//
// Author: Jonas Syrstad (jsyrstad2+StardustCore@gmail.com), http://no.linkedin.com/in/jonassyrstad/) 
// Copyright (c) 2014 Jonas Syrstad. All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Stardust.Interstellar
{
    public class CallStackItem
    {
        public override string ToString()
        {
            return string.Join(".", Name, MethodName);
        }

        private DateTime? TaskEndTime;

        public string AdditionalInformation { get; set; }

        public List<CallStackItem> CallStack { get; set; }

        public DateTime? EndTimeStamp
        {
            get
            {
                return TaskEndTime;
            }
            set
            {
                if (value.HasValue)
                {
                    ExecutionTime = (value.Value - TimeStamp).TotalMilliseconds;
                }
                else
                    ExecutionTime = null;
                TaskEndTime = value;
            }
        }

        public string ErrorPath { get; set; }

        public string ExceptionMessage { get; set; }

        public double? ExecutionTime { get; set; }

        public string MethodName { get; set; }

        public string Name { get; set; }

        public string StackTrace { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}