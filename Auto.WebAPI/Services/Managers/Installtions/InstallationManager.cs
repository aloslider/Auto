using Auto.WebAPI.Database.Repositories.Branches;
using Auto.WebAPI.Database.Repositories.Devices;
using Auto.WebAPI.Database.Repositories.Installations;
using Auto.WebAPI.Dtos;
using Auto.WebAPI.Models;
using static Prelude;

class InstallationManager(
    IInstallationRepository instRep,
    IBranchesRepository branchRep,
    IDevicesRepository devRep) : IInstallationsManager
{
    readonly IInstallationRepository _instRep = instRep;
    readonly IBranchesRepository _branchRep = branchRep;
    readonly IDevicesRepository _devRep = devRep;

    public async Task<Either<string, int>> CreateAsync(InstallationDto? installationDto)
    {
        if (!DtoIsValid(installationDto, out string? error))
        {
            return Left(error!);
        }

        // Check branch exists
        var branch = await _branchRep.GetByIdAsync(installationDto!.BranchId);

        if (branch is null)
        {
            return Left("Branch doesn't exist.");
        }

        // Check device exists
        var device = await _devRep.GetByIdAsync(installationDto.DeviceId);

        if (device is null)
        {
            return Left("Device doesn't exist.");
        }

        var branchInstallations = 
            (await _instRep.GetBranchInstallationsAsync(branch.Id))
            .OrderBy(i => i.OrderNumber);

        // Check same name exists
        if (branchInstallations.Where(i => i.Name == installationDto.Name).Any())
        {
            return Left("Installation with the same name already exists.");
        }

        // Check default exists
        if (installationDto.IsDefault)
        {
            if (branchInstallations.Where(i => i.IsDefault == true).Any())
            {
                return Left("Default installation already exists in the branch.");
            }
        }

        // Check same order number exists
        if (installationDto.OrderNumber.HasValue)
        {
            var instWithSameOrderNumber = 
                branchInstallations
                .Where(i => i.OrderNumber == installationDto.OrderNumber.Value)
                .SingleOrDefault();

            if (instWithSameOrderNumber is not null)
            {
                return Left("Installation with the same order number already exists in this branch.");
            }
        }
        else
        {
            var lastInstByOrderNumber = branchInstallations?.LastOrDefault();

            if (lastInstByOrderNumber is not null)
            {
                installationDto.OrderNumber = ++lastInstByOrderNumber.OrderNumber;
            }
            else
            {
                installationDto.OrderNumber = 1;
            }
        }

        int idCreated = _instRep.Create(new Installation()
        {
            Name = installationDto.Name!,
            Branch = branch,
            Device = device,
            OrderNumber = installationDto.OrderNumber.Value,
            IsDefault = installationDto.IsDefault,
        });
        return Right(idCreated);
    }

    public async Task DeleteAsync(int id)
    {
        var installationToDelete = await _instRep.GetByIdAsync(id);

        if (installationToDelete is null)
        {
            return;
        }

        List<Installation> otherBranchInstallations = 
            (await _instRep.GetBranchInstallationsAsync(installationToDelete.Branch.Id))
            .Where(i => i.Id != id)
            .OrderBy(i => i.OrderNumber)
            .ToList();

        if (installationToDelete.IsDefault && otherBranchInstallations.Count > 0)
        {
            Installation newDefaultInstallation = otherBranchInstallations.First();
            newDefaultInstallation.IsDefault = true;
            await _instRep.UpdateAsync(newDefaultInstallation.Id, newDefaultInstallation);
        }

        await _instRep.DeleteAsync(id);
    }

    public async Task<List<Installation>> GetAllAsync()
    {
        return await _instRep.GetAllAsync();
    }

    public async Task<Either<string, Installation>> GetByIdAsync(int id)
    {
        var installation = await _instRep.GetByIdAsync(id);

        return installation is not null
            ? Right(installation)
            : Left("Installation not found");
    }

    static bool DtoIsValid(InstallationDto? installation, out string? error)
    {
        if (installation is null)
        {
            error = "Body is null";
            return false;
        }

        if (string.IsNullOrEmpty(installation.Name))
        {
            error = "Name must be not null and non empty.";
            return false;
        }

        error = null;
        return true;
    }
}