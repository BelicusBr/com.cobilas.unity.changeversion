# Changelog
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