# Changelog

## [1.4.5]
### Mew
- Added [GameObject InstantiatePrefab(GameObject, Transform)] to IDependencyInjectionService

## [1.4.4]
### Changes
- Removed TMPRuntimeLocalizedText from LocalizationService
- Added RuntimeLocalizedTextSetAction as abstract way to set text
### Migration
- LocalizationService.ConvertToUnityRuntimeLocalizedTMP renamed to ConvertToRuntimeLocalizedText

## [1.4.3]
### New
- Added IAssetProvider to work with Addressables
- ZENJECT define will automatically added if detecs zenject package (only for git zenject package)

## [1.4.2]
### Changes
- Changes in Localization Service **[UnityLocalization]**, removed direct dependecy of _com.unity.localization_
- UnityLocalizationService enables after installing _com.unity.localization_ package 

## [1.4.1]
### New
- Introduced IStateMachine interface
- Added State Machine conditions to change States
- Fixed State machine [Exit] Method

## [1.4.0]
### New
- Fixed State machine [Exit] Method

## [1.3.9]
### New
- Fixed DiService Container update
- State machine state factory

## [1.3.8]
### New
- Added FSM
- Added ITick interface

## [1.3.7]
### New
- Added loging condition to DIService
- Added Pool pattern

## [1.3.4]
### Fix
- Sound Service fixes

## [1.3.1]
### Fix
- Fixed Send Request in Mediator
### New
- Added IRequestHandler without return type (void)

## [1.3.1]
### Fix
- Fixed Runtime Localization

## [1.3.1]
### Fix
- Fixed Runtime Localization

## [1.3.0]
### New
- Added Save Serices
- Added Handler to Patterns

## [1.2.0]
### New
- Added Localization Service
- Added UnityLocalization Implementation of Localization Service

## [1.0.9]
### New
- Changed dependency of https://github.com/mob-sakai/ParticleEffectForUGUI]
### Migration
- Add "COFFEE_PARTICLES" to ScriptingDefineSymbols to enable its implementation

## [1.0.8]
### New
- Added UI Service
- Removed Zenject dependencies
### Migration
- Add "ZENJECT" to ScriptingDefineSymbols to enable Zenject implementations

## [1.0.7]
### Changes
- Events to update AudioService states

## [1.0.6]
### Changes
- Added SetBackground to AudioService [Changes BaseBackgroundSource clip, not created new AudioSource]
