using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.Domain.SeedWork
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }
        public int Id { get; private set; }
        protected Enumeration(int id,string name)
        {
            Id = id;
            Name = name;
        }
        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
                return false;

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }
        public override int GetHashCode() => Id.GetHashCode();
        public static int AbsoluteDifference(Enumeration firstValue,Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }
        public static T FromValue<T>(int value) where T : Enumeration
        {
            var matchingItem = Parse<T, int>(value, "value", item => item.Id == value);
            return matchingItem;
        }
        private static T Parse<T,K>(K value, string description ,Func<T,bool> predicate) where T : Enumeration
        {
            var mathingItem = GetAll<T>().FirstOrDefault(predicate);

            if (mathingItem == null)
                throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
            return mathingItem;
        }
        public int CompareTo(object other) => Id.CompareTo(((Enumeration)other).Id);
    }
}
