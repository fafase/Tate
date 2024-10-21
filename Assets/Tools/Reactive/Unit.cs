namespace Rx 
{
    public sealed class Unit
    {
        public static readonly Unit Default = new Unit();
        private Unit() { }
        public override string ToString() => "Unit";
    }
}
