# (Thesis) Main Architecture Solution


## Architecture Overview
The soluiton implementation is split into 3 main modules for processing and 1 module for file storage and data persistence.

The main computing modules are:
  1. **Web**: A public UI that displays all media files in the system, along with the streaming and uploading functionalites.
  2. **Encoding Core:** A periodic background processor that coverts original video files to adaptive MPEG-Dash formats with multiple renditions (WEBM cotainer).
  3. **Streaming Core:** A HTTP server for the manifest files and media content.

The data module is a pure SQL store for holding metadata and tracking of media files during the their life cycle (upload, encode, stream), and file directory store for the original files and their DASH-MPEG encoded segments and manifestos.

The digram below depicts in high-level the architecture's design.

![HLD](https://raw.githubusercontent.com/abarrak/kubernetes-streaming-architecuture/master/solution/Design/Methodoloy-HLD.png)

## Soluiton Docker Images

- [streaming-system-web](https://hub.docker.com/r/abarrak/streaming-system-web/tags).
- [streaming-system-encoding-core](https://hub.docker.com/r/abarrak/streaming-system-encoding-core/tags).
- [streaming-system-streaming-core](https://hub.docker.com/r/abarrak/streaming-system-streaming-core/tags).

## Kubernetes Deployment

The [Deployment](https://github.com/abarrak/kubernetes-streaming-architecuture/tree/master/solution/Deployment) folder contains the primary suggested Kubernetes deployment script, along with other alternative to explore and compare.

## Notes
For further details and dicussion about the architectural aspects and analysis, please refer to the /evaluation folder and thesis publication.
