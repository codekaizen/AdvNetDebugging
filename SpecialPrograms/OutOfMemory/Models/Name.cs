using System;

namespace OutOfMemory.Models
{
    public sealed class Name : IEquatable<Name>
    {
        public Name(String individualName, String familyName)
        {
            IndividualName = individualName;
            FamilyName = familyName;
        }

        public String IndividualName { get; }
        public String FamilyName { get; }

        public Boolean Equals(Name other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return String.Equals(IndividualName, other.IndividualName) &&
                   String.Equals(FamilyName, other.FamilyName);
        }

        public override Boolean Equals(Object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as Name);
        }

        public override Int32 GetHashCode()
        {
            unchecked
            {
                return ((IndividualName?.GetHashCode() ?? 0) * 397) ^ (FamilyName?.GetHashCode() ?? 0);
            }
        }

        public static Boolean operator ==(Name left, Name right)
        {
            return Equals(left, right);
        }

        public static Boolean operator !=(Name left, Name right)
        {
            return !Equals(left, right);
        }
    }
}