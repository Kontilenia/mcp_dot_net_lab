# Petstore MCP Lab for Visual Studio

This lab demonstrates how to use GitHub Copilot, Model Context Protocol (MCP)
tools, agent skills, repository instructions, and prompt files together in
Visual Studio.

The lab has three steps:

1. Configure the MCP server and use it from GitHub Copilot.
2. Enable agent skills and GitHub Copilot instructions.
3. Create and validate an API by using skills, prompts, and MCP tools.

> [!IMPORTANT]
> This lab uses the public
> [Swagger Petstore API](https://petstore.swagger.io/). Use the service
> responsibly and avoid sending excessive or inappropriate requests.

## Prerequisites

Before starting, make sure you have:

- Visual Studio with GitHub Copilot installed and signed in.
- An active GitHub Copilot license.
- The .NET 10 SDK.
- Outbound access to `api.nuget.org` and `petstore.swagger.io`.
- This repository opened in Visual Studio.

## Step 1: Configure and use the MCP server

### 1. Review the available GitHub Copilot tools

1. Open **GitHub Copilot Chat**.
2. Select **Agent** mode.
3. Select **Interactive** for confirmations and **Auto** for the model.
4. Select the **Tools** icon.
5. Review the built-in and currently added tools.

### 2. Prepare the MCP configuration for Visual Studio

For this part, using **Folder View** together with **Show All Files** is
recommended. This makes it easier to see configuration folders and files that
may not appear in Solution View.

1. Locate `.vscode/mcp.json`.
2. Move the file into the `PetManagement` folder.
3. Rename it to `.mcp.json`.
4. Update the MCP project path to:

   ```text
   src/PetMcpServer/PetMcpServer.csproj
   ```

   The project argument in `.mcp.json` should therefore point to the MCP
   server relative to the `PetManagement` folder.

5. Save `.mcp.json`.

After saving, Visual Studio should display an action above the server
configuration. Select **Configure**, then **Enable and Start Server**.

Under **Servers**, confirm that:

- The `petstore` server shows **Running**.
- The server exposes **2 assets**.

### 3. Explore the Petstore MCP tools

1. Return to **GitHub Copilot Chat**.
2. Confirm that **Agent**, **Interactive**, and **Auto** are still selected.
3. Select the **Tools** icon again.
4. Under the added tools, confirm that the two Petstore MCP tools are listed.
5. If either tool is disabled, enable it.
6. In the tools view, open the MCP server section and review the two Petstore
   tools:
   - **Get pet by ID** retrieves a pet by its ID.
   - **Add pet** creates a pet by using a name and status.

### 4. Use the MCP tools from Copilot Chat

Ask GitHub Copilot to create a pet. For example:

```text
Use the Petstore MCP tool to create a pet named VisualStudioPet with the
status available.
```

While the request runs:

1. Monitor the GitHub Copilot output.
2. Confirm that Copilot uses the MCP tool to create the pet.
3. Record the returned pet ID.

Next, use that ID to retrieve the pet:

```text
Use the Petstore MCP tool to get the pet with ID <pet-id>.
```

Confirm that the returned details match the pet you created. You can also
explore the same public service through the
[Swagger Petstore page](https://petstore.swagger.io/).

### Step 1 checkpoint

Before continuing, confirm that:

- The MCP server is running.
- Both Petstore tools are enabled in GitHub Copilot Chat.
- You created a pet and received its ID.
- You retrieved that pet by its ID.

## Step 2: Enable skills and GitHub Copilot instructions

### 1. Review existing skills

1. Return to **GitHub Copilot Chat**.
2. Select the **Tools** icon.
3. Open the **Skills** section.
4. Review the user and add-on skills. Some may be enabled while others may be
   disabled.
5. Select a skill and choose **Edit** to see how a skill is structured.
6. Do not change the skill, only review it.

### 2. Enable the workspace skills

1. In the repository, open `.github/not_yet_skills`.
2. Review the Markdown files inside the skill folders.
3. Rename the `not_yet_skills` folder to `skills`.
4. Return to **GitHub Copilot Chat**.
5. Select **Tools**, then open **Skills**.
6. Confirm that the workspace skills now appear.
7. Enable the workspace skills if they are not already enabled.

When enabled, the skills are available for the selected language model to use
when a request matches their descriptions.

### 3. Enable the repository-wide Copilot instructions

1. In the `.github` folder, open
   `not_copilot-instructions_yet.md`.
2. Review the general instructions without changing their content.
3. Rename the file to `copilot-instructions.md`.
4. Confirm that the file icon changes and it matches the icon of the other skills.

These instructions are now available to GitHub Copilot for every request made
in this repository.

### Step 2 checkpoint

Before continuing, confirm that:

- `.github/skills` contains the two workspace skills.
- Both workspace skills appear in GitHub Copilot Chat.
- `.github/copilot-instructions.md` is recognized as a Copilot instructions
  file.

## Step 3: Create and validate an API

### 1. Write the API prompt

1. Open `.github/prompts/prompt.md`.
2. Add the following refined prompt:

   ```text
   Create a new ASP.NET Core API in .NET that uses the public Swagger Petstore
   API. Add an endpoint that accepts a pet ID, retrieves that pet from the
   Swagger Petstore, and returns true when the pet's status is "available" or
   false when it has any other status.
   ```

3. Run the prompt with GitHub Copilot in the existing chat.

### 2. Observe GitHub Copilot while it works

As the agent runs:

1. Check whether it uses the two workspace skills enabled in Step 2.
2. Review any request to access the Swagger Petstore page.
3. Review any request to build or test the solution.
4. Grant access only when the requested action is expected and appropriate for
   the lab.
5. Wait for the implementation to finish.

### 3. Review and refine the generated implementation

After GitHub Copilot finishes:

1. Use the prompt in `.github/prompts` in the same chat to ask GitHub Copilot
   to explain the code it generated.
2. Review all changes in the generated code files yourself.
3. Check whether GitHub Copilot added tests or made other improvements.
4. Decide whether to accept the changes, reject them, or ask for refinements.

You can also adjust the skills or rewrite the prompt and run it again if you
want to explore a different result.

### 4. Create test data with different statuses

You created one pet in Step 1. Open a separate GitHub Copilot chat and use the
MCP server to create another pet with a different status.

For example, if the first pet was `available`, create another pet with a status
of `sold` or `pending`:

```text
Use the Petstore MCP tool to create a pet with the
status sold.
```

Record the IDs of both pets.

### 5. Run and validate the generated API

Return to the chat that generated the API and ask:

```text
How do I start the API and invoke the new endpoint?
```

Follow the provided steps. Call the endpoint with both pet IDs and confirm
that:

- The pet with the `available` status produces `true`.
- The pet with the `sold` or `pending` status produces `false`.

### Bonus: Ask Copilot to validate through MCP

Ask GitHub Copilot to use the MCP server while testing the generated endpoint.
For example:

```text
Use the Petstore MCP tools to verify the two pets and then test the API
endpoint with their IDs. Confirm that the endpoint returns the expected
availability result for each pet.
```

## Outcomes

After completing the lab, you should be able to demonstrate:

- Less context switching by using GitHub Copilot features directly in Visual
  Studio.
- Shared automation and reusable guidance through skills, MCP servers, and
  prompt files.
- Repository-wide consistency through GitHub Copilot instructions.
- Validation of generated code by combining MCP tools with normal API testing.
- Increased productivity when creating, reviewing, refining, and validating
  .NET solutions.
