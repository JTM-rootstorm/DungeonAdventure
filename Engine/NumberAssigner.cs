namespace Engine
{
    public static class NumberAssigner
    {
        static int _nextNumer = 0;

        public static int getNextNumber()
        {
            _nextNumer = (_nextNumer + 1);

            return _nextNumer;
        }
    }
}
