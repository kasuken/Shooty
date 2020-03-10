<script>
    $(document).ready(function () {
        Dropzone.options.uploader = {
            paramName: "file",
            maxFilesize: 2,
            accept: function (file, done) {
                if (file.name === "test.jpg") {
                    alert("Can't upload a test file.");
                }
                else {
                    //Show a confirm alert and display the image on the page.
                }
            }
        }
});
</script>