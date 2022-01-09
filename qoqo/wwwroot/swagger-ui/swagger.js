function onLoad(swaggerUi) {
    const frontLink = document.createElement("a")
    frontLink.innerText = "Go to QoQo app"
    frontLink.style.textDecoration = "underline"
    frontLink.style.padding = "16px"
    frontLink.style.display = "inline-block"
    frontLink.style.color = "#2e86de"
    frontLink.style.textDecorationThickness = "2px"

    frontLink.href = "https://localhost:7257";

    swaggerUi.insertBefore(frontLink, swaggerUi.children[1])
}

function run() {
    const swaggerUi = document.querySelector(".swagger-ui.swagger-container")
    onLoad(swaggerUi)
}

setTimeout(run, 200)
