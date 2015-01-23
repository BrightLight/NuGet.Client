﻿using NuGet.Packaging;
using NuGet.PackagingCore;
using NuGet.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NuGet.PackageManagement
{
    public interface IPackageRestoreManager
    {
        /// <summary>
        /// Gets a value indicating whether the current solution is configured for Package Restore mode.
        /// </summary>
        bool IsCurrentSolutionEnabledForRestore { get; }

        /// <summary>
        /// Configures the current solution for Package Restore mode.
        /// </summary>
        /// <param name="fromActivation">if set to <c>false</c>, the method will not show any error message, and will not set package restore consent.</param>
        void EnableCurrentSolutionForRestore(bool fromActivation);

        /// <summary>
        /// Occurs when it is detected that the packages are missing or restored for the current solution.
        /// </summary>
        event EventHandler<PackagesMissingStatusEventArgs> PackagesMissingStatusChanged;

        event EventHandler<PackageRestoredEventArgs> PackageRestoredEvent;

        IEnumerable<PackageReference> GetMissingPackagesInSolution();

        IEnumerable<PackageReference> GetMissingPackages(NuGetProject nuGetProject);

        /// <summary>
        /// Checks the current solution if there is any package missing.
        /// </summary>
        /// <returns></returns>
        void RaisePackagesMissingEventForSolution();

        /// <summary>
        /// Restores the missing packages for the current solution. Returns true if atleast one package was restored
        /// </summary>
        Task<bool> RestoreMissingPackagesInSolution();

        /// <summary>
        /// Restores the missing packages for a project. Returns true if atleast one package was restored
        /// </summary>
        Task<bool> RestoreMissingPackages(NuGetProject nuGetProject);

        /// <summary>
        /// Restores the passed in missing packages. Returns true if atleast one package was restored
        /// </summary>
        // TODO : Use IEnumerable<PackageIdentity> instead of IEnumerable<PackageReference>
        Task<bool> RestoreMissingPackages(IEnumerable<PackageReference> packageReferences);
    }

    /// <summary>
    /// To be raised when package restore for 'Package' did not fail
    /// If 'Restored' is false, it means that the package was already restored
    /// If 'Restored' is true, the package was restored and successfully
    /// </summary>
    public class PackageRestoredEventArgs : EventArgs
    {
        public PackageIdentity Package { get; private set; }
        public bool Restored { get; private set; }
        public PackageRestoredEventArgs(PackageIdentity packageIdentity, bool restored)
        {
            Package = packageIdentity;
            Restored = restored;
        }
    }

    public class PackagesMissingStatusEventArgs : EventArgs
    {
        public bool PackagesMissing { get; private set; }

        public PackagesMissingStatusEventArgs(bool packagesMissing)
        {
            PackagesMissing = packagesMissing;
        }
    }
}