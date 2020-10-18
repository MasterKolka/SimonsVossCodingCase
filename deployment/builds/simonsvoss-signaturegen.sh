TAG="latest"
IMAGE="simonsvoss-signaturegen"

REGISTRYNAME="simonsvosstestcontainers"
REGISTRYSERVER="${REGISTRYNAME}.azurecr.io"
REGISTRYPW=""

TAG="${REGISTRYSERVER}/${IMAGE}:${TAG}"

docker login $REGISTRYSERVER -u $REGISTRYNAME -p $REGISTRYPW

docker build --tag $TAG -f ../../src/SimonsVossCodingCase.LicenceSignatureGenerator/Dockerfile ../../src/SimonsVossCodingCase.LicenceSignatureGenerator
docker push $TAG