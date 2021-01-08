# Dotnet Namespaces
## Gliederung

Die Solution wird in 3 Projekte aufgeteilt:
- Core Library: Enthält Shared Komponenten wie z.B. den Renderer oder Grpc Klassen
- Node Console App: Die Node Applikation die auf den projektions Computer läuft. 
- Controller Console App: Die Controller Applikation, die die einzelnen Nodes fernsteuert und die Weboberfläche bereistellt.
- Angular Applikation: Der Controller stellt die Angular Applikation bereit. 


## Namespaces
- Lightning
- Lightning.Core
  - Lightning.Core.Rendering
    - Lightning.Core.Rendering.Layers
    - Lightning.Core.Rendering.Transformations
    - Lightning.Core.Rendering.ColorCorrection
    - Lightning.Core.Rendering.Blending
  - Lightning.Core.MediaSync
  - Lightning.Core.TimeSync
  - Lightning.Core.Controlflow
  - Lightning.Core.LayerEdit

- Lightning.Node
  - Lightning.Node.Controllers

- Lightning.Controller
  - Lightning.Controller.Controllers