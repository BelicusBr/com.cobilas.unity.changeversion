# Changelog
O formato é baseado em [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
e este projeto segue o [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 01/09/2023
### Obsoleto
- O arquivo <kbd>VersionConfig.json</kbd> não e mas utilizado pelo <kbd>ChangeVersion</kbd>, mas o arquivo não será apagado.
### Added
- A nova função `Tools/ChangeVersion/Open ChangeVersion folder` foi adicionada.
- As opções customizadas foram adicionadas como
```c#
sealed class PreProductionCharacterOption;
sealed class UpdateBuildOption;
sealed class UpdateClosedOption;
sealed class UpdateRevisionOption;
```
### Removed
- As classes 
```c#
class BaseBuildPhaseTemplate;
class BaseVersionTemplate;
static class ParseOldChangeVersion
```
foram removidas.
## [1.3.0] - 29/08/2023
## Changed
- As dependencias do pacote foram aluteradas.
## [1.2.0-ch1] - 28/08/2023
### Changed
- O autor do pacote foi alterado de `Cobilas CTB` para `BélicusBr`.
## [1.1.1] - 21/02/2023
### Fixed
Agora cada item no `ChangeVersion` pode ser renomeado.
## [1.0.3] - 30/01/2023
### Changed
- Os métodos `bool:ChangeVersionWin.ToolbarButton(string, float)` e `bool:ChangeVersionWin.ToolbarButton(string)` foram comentados por não serem usados.

## [1.0.2] 23/11/2022
### Fix (Informações perdidas)
A o abrir a janela 'ChangeVersion' os arquivos 'ChangeVersion/VersionConfig.txt e 'ChangeVersion/Config.txt' 
eram apagados mais as informações não eram absorvidos pelo 'ChangeVersion' o que ocasionava num valor vaziu
padrão.
### Fix (Biblioteca duplicada)
A biblioteca duplicada `using System.Collections.Generic;` foi removida de
'Editor\Change version\Class's\VersionTemplate.cs(Ln:3)'
## [1.0.0] 10/11/2022
### Repositorio com.cobilas.unity.utility iniciado
- Lançado para o GitHub