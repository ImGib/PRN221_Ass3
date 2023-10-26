$(() => {

    var connection = new signalR.HubConnectionBuilder().withUrl("/signalrServer").build();
    connection.start();

    connection.on("LoadPosts", function (id) {
        LoadProdData(id);
    })

    const LoadProdData = (id) => {
        var tr = '';
        $.ajax({
            url: 'Posts/GetPosts',
            method: 'GET',
            data: {id:id},
            success: (result) => {
                console.log(result)
                tr += `<tr>
                    <td> ${result.authorID} </td>
                    <td> ${result.createdDate} </td>
                    <td> ${result.updatedDate} </td>
                    <td> ${result.title} </td>
                    <td> ${result.content} </td>
                    <td> ${result.publishStatus} </td>
                    <td> ${result.categoryID} </td>
                    <td>
                        <a href='../Posts/Edit?id=${id}'>Edit</a> |
                        <a href='../Posts/Details?id=${id}'>Details</a> |
                        <a href='../Posts/Delete?id=${id}'>Delete</a>
                    </td>
                    </tr>`;

                $("#post_" + id).html(tr);
            },
            error: (error) => {
                console.log(error)
            }
        });
    }
})