TAG="latest"
IMAGE="simonsvoss-registration"

REGISTRYNAME="simonsvosstestcontainers"
REGISTRYSERVER="${REGISTRYNAME}.azurecr.io"
REGISTRYPW=""

TAG="${REGISTRYSERVER}/${IMAGE}:${TAG}"

docker login $REGISTRYSERVER -u $REGISTRYNAME -p $REGISTRYPW

docker build --tag $TAG -f ../../src/SimonsVossCodingCase.RegistrationService/Dockerfile ../../src/SimonsVossCodingCase.RegistrationService
docker push $TAG