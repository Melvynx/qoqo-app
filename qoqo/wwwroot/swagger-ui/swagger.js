function addLink(swaggerUi) {
    const frontLink = document.createElement("a")
    frontLink.innerText = "Go to QoQo app"

    const style = `
        background-color: #1B1B1B;
        color: white;
        padding: 16px 32px;
        text-align: center;
        border: 2px solid #85EA2C;
        display: inline-block;
        text-decoration: none;
        border-radius: 9999px;
        margin: 16px 16px 0 16px;`
    Object.assign(frontLink.style, cssFormat(style))

    frontLink.href = "https://localhost:7257";

    swaggerUi.insertBefore(frontLink, swaggerUi.children[1])
}

function run() {
    const swaggerUi = document.querySelector(".swagger-ui.swagger-container")
    addLink(swaggerUi)
}

const cssFormat = (value) => value.split(";")
    .map(v => v.trim())
    .filter(v => Boolean(v))
    .reduce((acc, curr) => {
        const [key, value] = curr.split(":")
        acc[key.trim()] = value.trim()
        return acc
    }, {})

setTimeout(run, 200)
