﻿using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggregatesModel
{
    public class ProjectProperty : ValueObject
    {
        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public int ProjectId { get; set; }

        public ProjectProperty() { }
        public ProjectProperty(string key,string text ,string value)
        {
            Key = key;
            Text = text;
            Value = value;
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Key;
        }
    }
}
