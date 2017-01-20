﻿using System.Linq;
using NLog;

namespace cafe.CommandLine
{
    public class Runner
    {
        private static readonly Logger Logger = LogManager.GetLogger(typeof(Runner).FullName);

        private readonly Option[] _options;

        public Runner(params Option[] options)
        {
            _options = options;
        }

        public int Run(params string[] args)
        {
            if (string.Equals(args.FirstOrDefault(), "-h"))
            {
                ShowHelpForAllOptions();
            }
            var matchingOption = MatchingOptionFor(args);

            if (matchingOption != null)
            {
                Logger.Info($"Option {matchingOption} matches the arguments supplied, so running");
                var result = matchingOption.Run(args);
                Logger.Info($"Finished executing {matchingOption} with result: {result}");
                return result.IsSuccess ? 0 : -1;
            }
            else
            {
                Presenter.ShowError("No options match the supplied arguments. Run -h to view all options", Logger);
                return -2;
            }
        }

        private void ShowHelpForAllOptions()
        {
            Presenter.ShowMessage($"Showing help for all {_options.Length} options", Logger);
            foreach (var option in _options)
            {
                option.ShowHelp();
            }
        }

        private Option MatchingOptionFor(string[] args)
        {
            foreach (var option in _options)
            {
                if (option.IsSatisfiedBy(args))
                {
                    Logger.Debug($"Option {option} matches arguments supplied");
                    return option;
                }
                else
                {
                    Logger.Info($"Option {option} does not satisfy the arguments supplied");
                }
            }
            return null;
        }
    }
}