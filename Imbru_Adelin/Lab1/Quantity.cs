using CSharp.Choices;

namespace Lab1
{
    [AsChoice]
    public static partial class Quantity{
        public interface IQuantity{}
        public record Units (int number):IQuantity;
        public record Kilograms (double numberOfKg):IQuantity;
        public record Undefined (string undefined):IQuantity;
    }
}