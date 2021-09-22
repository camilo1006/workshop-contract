namespace ContractTest
{
    using System;

    public class ContractInfo
    {
        public static string _pactUrl = "https://perfi.pactflow.io";
        public static string _token = "AILOfdOj-GIN_uOWf4KRmg";

        public static string _consumerVersion = $"version-{new Random().Next(0, 100)}";
        public static string _providerVersion = $"version-{new Random().Next(0, 100)}";
    }
}
