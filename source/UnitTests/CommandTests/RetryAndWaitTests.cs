namespace Archiver.Tests
{
    using System.Diagnostics;
    using System.Reflection;
    using Xunit;

    public class InvokerRetryTests
    {
        [Fact]
        public void ExecuteCommand_ExecutesCommandOnce_WhenFirstAttemptSucceeds()
        {
            TestCommand command = new TestCommand(
                call => this.CreateSuccessResult());

            Invoker invoker = this.CreateInvoker(command);

            ParsedArguments arguments = this.CreateArguments(
                retryAmount: 3,
                waitTime: TimeSpan.FromMilliseconds(10));

            Result result = invoker.ExecuteCommand(arguments);

            Assert.False(result.ErrorOccured);
            Assert.Equal(1, command.ExecuteCount);
        }

        [Fact]
        public void ExecuteCommand_RetriesCommand_WhenFirstAttemptThrowsException()
        {
            TestCommand command = new TestCommand(
                call =>
                {
                    if (call == 1)
                    {
                        throw new IOException();
                    }

                    return this.CreateSuccessResult();
                });

            Invoker invoker = this.CreateInvoker(command);

            ParsedArguments arguments = this.CreateArguments(
                retryAmount: 3,
                waitTime: TimeSpan.FromMilliseconds(10));

            Result result = invoker.ExecuteCommand(arguments);

            Assert.False(result.ErrorOccured);
            Assert.Equal(2, command.ExecuteCount);
        }

        [Fact]
        public void ExecuteCommand_StopsRetrying_WhenAttemptSucceeds()
        {
            TestCommand command = new TestCommand(
                call =>
                {
                    if (call < 3)
                    {
                        throw new IOException();
                    }

                    return this.CreateSuccessResult();
                });

            Invoker invoker = this.CreateInvoker(command);

            ParsedArguments arguments = this.CreateArguments(
                retryAmount: 5,
                waitTime: TimeSpan.FromMilliseconds(10));

            Result result = invoker.ExecuteCommand(arguments);

            Assert.False(result.ErrorOccured);
            Assert.Equal(3, command.ExecuteCount);
        }

        [Fact]
        public void ExecuteCommand_DoesNotExecuteMoreOftenThanRetryAmount()
        {
            TestCommand command = new TestCommand(
                call => throw new IOException());

            Invoker invoker = this.CreateInvoker(command);

            ParsedArguments arguments = this.CreateArguments(
                retryAmount: 3,
                waitTime: TimeSpan.FromMilliseconds(10));

            Result result = invoker.ExecuteCommand(arguments);

            Assert.True(result.ErrorOccured);
            Assert.Equal(3, command.ExecuteCount);
        }

        [Fact]
        public void ExecuteCommand_WaitsBetweenFailedAttempts()
        {
            TestCommand command = new TestCommand(
                call =>
                {
                    if (call == 1)
                    {
                        throw new IOException();
                    }

                    return this.CreateSuccessResult();
                });

            Invoker invoker = this.CreateInvoker(command);

            TimeSpan waitTime = TimeSpan.FromMilliseconds(100);

            ParsedArguments arguments = this.CreateArguments(
                retryAmount: 2,
                waitTime: waitTime);

            Stopwatch stopwatch = Stopwatch.StartNew();

            invoker.ExecuteCommand(arguments);

            stopwatch.Stop();

            Assert.True(stopwatch.Elapsed >= waitTime);
        }

        [Fact]
        public void ExecuteCommand_DoesNotWait_WhenFirstAttemptSucceeds()
        {
            TestCommand command = new TestCommand(
                call => this.CreateSuccessResult());

            Invoker invoker = this.CreateInvoker(command);

            ParsedArguments arguments = this.CreateArguments(
                retryAmount: 3,
                waitTime: TimeSpan.FromMilliseconds(200));

            Stopwatch stopwatch = Stopwatch.StartNew();

            invoker.ExecuteCommand(arguments);

            stopwatch.Stop();

            Assert.True(stopwatch.Elapsed < TimeSpan.FromMilliseconds(200));
        }

        [Fact]
        public void ExecuteCommand_WaitsOnlyBetweenRetries()
        {
            TestCommand command = new TestCommand(
                call => throw new IOException());

            Invoker invoker = this.CreateInvoker(command);

            TimeSpan waitTime = TimeSpan.FromMilliseconds(100);

            ParsedArguments arguments = this.CreateArguments(
                retryAmount: 2,
                waitTime: waitTime);

            Stopwatch stopwatch = Stopwatch.StartNew();

            invoker.ExecuteCommand(arguments);

            stopwatch.Stop();

            Assert.Equal(2, command.ExecuteCount);

            Assert.True(
                stopwatch.Elapsed >= TimeSpan.FromMilliseconds(100));

            Assert.True(
                stopwatch.Elapsed < TimeSpan.FromMilliseconds(190));
        }

        private Invoker CreateInvoker(ICommand command)
        {
            Invoker invoker = new Invoker();

            FieldInfo? commandField = typeof(Invoker).GetField(
                "command",
                BindingFlags.Instance | BindingFlags.NonPublic);

            Assert.NotNull(commandField);

            commandField.SetValue(invoker, command);

            return invoker;
        }

        private ParsedArguments CreateArguments(
            byte retryAmount,
            TimeSpan waitTime)
        {
            return new ParsedArguments(
                new NoCompress(),
                retryAmount,
                waitTime,
                "source",
                "destination",
                MainCommands.Extract);
        }

        private Result CreateSuccessResult()
        {
            return new Result(
                false,
                ConsoleErrorOutput.WriteNoErrorOccurred,
                "test");
        }

        private class TestCommand : ICommand
        {
            private readonly Func<int, Result> execute;

            public TestCommand(Func<int, Result> execute)
            {
                this.execute = execute;
            }

            public int ExecuteCount
            {
                get;
                private set;
            }

            public Result Execute(ParsedArguments parsedArguments)
            {
                this.ExecuteCount++;

                return this.execute(this.ExecuteCount);
            }
        }
    }
}