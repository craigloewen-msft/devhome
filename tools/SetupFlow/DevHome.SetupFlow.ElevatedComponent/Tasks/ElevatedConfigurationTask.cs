﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using DevHome.SetupFlow.Common.Configuration;
using DevHome.SetupFlow.Common.Helpers;
using DevHome.SetupFlow.ElevatedComponent.Helpers;
using Microsoft.Management.Configuration;
using Windows.Foundation;
using Windows.Win32.Foundation;

namespace DevHome.SetupFlow.ElevatedComponent.Tasks;

public sealed class ElevatedConfigurationTask
{
    public IAsyncOperation<ElevatedConfigureTaskResult> ApplyConfiguration(string filePath, string content, Guid activityId)
    {
        return Task.Run(async () =>
        {
            var taskResult = new ElevatedConfigureTaskResult();

            try
            {
                var configurationFileHelper = new ConfigurationFileHelper(activityId);

                Log.Logger?.ReportInfo(Log.Component.Configuration, $"Opening configuration set from file: {filePath}");
                await configurationFileHelper.OpenConfigurationSetAsync(filePath, content);

                Log.Logger?.ReportInfo(Log.Component.Configuration, "Starting configuration set application");
                var result = await configurationFileHelper.ApplyConfigurationAsync();
                Log.Logger?.ReportInfo(Log.Component.Configuration, "Configuration application finished");

                taskResult.TaskAttempted = true;
                taskResult.TaskSucceeded = result.Succeeded;
                taskResult.RebootRequired = result.RequiresReboot;
                taskResult.UnitResults = result.Result.UnitResults.Select(unitResult =>
                {
                    unitResult.Unit.Settings.TryGetValue("description", out var descriptionObj);
                    return new ElevatedConfigureUnitTaskResult
                    {
                        Type = unitResult.Unit.Type,
                        Id = unitResult.Unit.Identifier,
                        UnitDescription = descriptionObj?.ToString() ?? string.Empty,
                        Intent = unitResult.Unit.Intent.ToString(),
                        IsSkipped = unitResult.State == ConfigurationUnitState.Skipped,
                        HResult = unitResult.ResultInformation?.ResultCode?.HResult ?? HRESULT.S_OK,
                        ResultSource = (int)(unitResult.ResultInformation?.ResultSource ?? ConfigurationUnitResultSource.None),
                        ErrorDescription = unitResult.ResultInformation?.Description,
                    };
                }).ToList();

                if (result.ResultException != null)
                {
                    throw result.ResultException;
                }
            }
            catch (Exception e)
            {
                Log.Logger?.ReportError(Log.Component.Configuration, $"Failed to apply configuration.", e);
                taskResult.TaskSucceeded = false;
            }

            return taskResult;
        }).AsAsyncOperation();
    }
}
