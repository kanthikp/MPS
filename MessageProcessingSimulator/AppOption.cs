namespace MessageProcessingSimulator
{
    public class AppOption
    {
        public int MessageTypesCount { get; set; } = 26;
        public int MessageGeneratorIntervalInMilliSecs { get; set; } = 10;
        public int MessageConsumerIntervalInMilliSecs { get; set; } = 20;

        public string LogFileDirectory { get; set; } = @"../../../Logfiles";
    }
}