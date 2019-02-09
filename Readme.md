# Roslyn based condition parser

Demo implementation that reads conditions from a config file and processes them via roslyn based on the application state.

Demonstrates a usecase for roslyn scripting that allows a user to define custom conditions that are processed by the application.

___

# Security notice

Running arbitrary strings through the roslyn API is a potential security risk.

**Please don't ever put this code on your server and/or accept remote user input to it!**

Roslyn has access to the entire host context (= the running application).

If you put this on a server, you might as well expose your entire server publicly via FTP..

With .Net there is the possiblity of sandboxing (via a seperate AppDomain) but this isn't available for .Net Core (yet).

My sample thus executes with full access to the host context.

See the last condition (commented out) in [config.json](https://github.com/MarcStan/RoslynConditionParser/blob/master/RoslynConditionParser/config.json) for an example of what this means.

___

## Motivation

Laziness.

With roslyn, the DSL is C# and C# has an excellent syntax for boolean conditions.

So I didn't have to come up with my own grammar and also didn't have to write 2000+ lines of parser generator code and evaluating the resulting expressions with even more code.

The roslyn implementation has less than 200 lines of code and the documentation can be as short as "C# boolean expressions accepted".

## Usecase

My primary usecase (and reason for implementing this):

### Config based trigger/action system

I wanted to allow users to define conditions in a config file and have them process during runtime.

The application runs on the users system and I put restrictions into place for security reasons:

* Config file resides on local disk and is not synced with the cloud
* The user must manually edit the config file as the application does not have write access to it
* No plan for allowing the user to remotely edit the config/conditions (e.g. via app)

With these restrictions roslyn scripting is safe, as any attacker would first have to gain system level access (at which point securing the scripting API won't do any good anyway).
