let canvases = document.getElementsByTagName("canvas");
for (let canvas of canvases) {
    let shaderPath = canvas.getAttribute("id");
    const gl = canvas.getContext("webgl");
    // Set clear color to black, fully opaque
    gl.clearColor(0.0, 0.0, 0.0, 1.0);
    // Clear the color buffer with specified clear color
    gl.clear(gl.COLOR_BUFFER_BIT);
};