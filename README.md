# Audio-editor

Accessible WPF audio editor (MVP) in C# targeting .NET 8.

Features included in this initial commit:
- Open WAV/MP3 files
- Play / Pause / Stop using NAudio
- Basic trim (export a selected range)
- Accessibility: AutomationProperties on controls, keyboard shortcuts and clear status updates for screen readers
- CI: GitHub Actions build workflow (Windows)

MSIX packaging:
- This repo is prepared for MSIX packaging, but producing a signed MSIX in CI requires a signing certificate/secret. See packaging instructions in packaging.md.

Build & run (dev machine):
- Install .NET 8 SDK and Visual Studio 2022/2023 with WPF workload
- From repo root: `dotnet restore ./src/AudioEditor/AudioEditor.csproj`
- `dotnet build ./src/AudioEditor/AudioEditor.csproj -c Release`
- Open the project in Visual Studio to run/debug the WPF app.
