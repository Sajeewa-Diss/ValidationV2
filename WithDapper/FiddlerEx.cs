using System;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WithDapper;
using WithDapper.Models;

public class FiddlerEx : IFiddlerEx
{
    private readonly ConnectionStringHolder _connectionStringHolder;

    public FiddlerEx(ConnectionStringHolder connectionStringHolder)
    {
        _connectionStringHolder = connectionStringHolder;
    }

    public async Task<string> DoDapperStuff1(int personId)
    {
        string sqlOrderDetails = "SELECT TOP 5 * FROM DapperOrderDetail;";
        string sqlOrderDetail = "SELECT * FROM DapperOrderDetail WHERE OrderDetailID = @OrderDetailID;";
        string sqlCustomerInsert = "INSERT INTO DapperCustomer (CustomerName) Values (@CustomerName);";

        string msg = "";

        using (var conn = new SqlConnection(_connectionStringHolder.Value))
        {
            var orderDetails = await conn.QueryAsync<DapperOrderDetail>(sqlOrderDetails); //note we're running async version of each dapper method.
            var orderDetail = await conn.QueryFirstOrDefaultAsync<DapperOrderDetail>(sqlOrderDetail, new { OrderDetailID = 1 });
            var affectedRows = await conn.ExecuteAsync(sqlCustomerInsert, new { CustomerName = "Yoko", foo = "bar" }); //an anonymous object param (second param unused here).

            msg = $"rows returned: {orderDetails.Count().ToString()}. {orderDetail.ToString()}, and customer rows added = {affectedRows.ToString()}";
            return msg;
        }
        //return "unreachable code";
    }


    public async Task<string> DoDapperStuff2(int personId)
    {
        string msg = "";

        using (var conn = new SqlConnection(_connectionStringHolder.Value))
        {
            //Return two queries and combine (this metho dhas both sync and async versions).
            var multi = await conn.QueryMultipleAsync("GetCarDataSp1", new { CustomerID = personId },
                                commandType: CommandType.StoredProcedure);

            var cars = multi.Read<CarDTO>();
            var options = multi.Read<CarOptionDTO>();

            //wire the options to the cars
            foreach (var car in cars)
            {
                var carOptions = options.Where(opt => opt.CarID == car.CarID);
                car.CarOptions = carOptions.ToList();

                msg += $"car: {car.Name} has {car.CarOptions.Count} options \r\n";
            }
            
            return msg;
        }
        //return "unreachable code";
    }

    //async methods can't return out params so can instead combine multiple values into tuples, say and return this as a return type
    //code below is example of multi mapping. 
    public string MultiMapEx1(int personId)
    {
        string msg = "";

        using (var conn = new SqlConnection(_connectionStringHolder.Value))
        {
            var query1 = @"select productid, productname, p.categoryid, categoryname 
                from products p 
                inner join categories c on p.categoryid = c.categoryid"; //query params should be in an exact order and note where the split occurs for all following params to be sent into next object!!

            //Query<a,b,n> is a Func<> generic delegate where the generic type "a,b,c" etc are the inputs and "n" (the final param) is the output object type .
            var products = conn.Query<Product, Category, Product>(query1, (product, category) => {    //lambda func defines how the inputs (product and category) are used by the func delegate.
                product.Category = category;                                                          //this expression states product category is taken from category object, and then return the product object.
                return product;}, splitOn: "CategoryId");  //return type is Product and this will be mapped to param "n" in expression Query<a,b,n> above.
                                                           //the data from the query is sent as the input for product (first param) until splitOn param. At this place all following params are sent to next object (category).
                                                           //when having multiple splitOn params, use commas with no spaces:- splitOn: "col1,colb,colc"
            products.ToList().ForEach(product => msg+= $"Product: {product.ProductName}, Category: {product.Category.CategoryName}");
            return msg;
        }
        //return "unreachable code";
    }

    public async Task<string> MultiMapEx2(int personId)
    {
        string msg = "";
//#nullable disable
        using (var conn = new SqlConnection(_connectionStringHolder.Value))
        {
            var query2 = @"select p.postid, headline, t.tagid, tagname
                from posts p 
                inner join posttags pt on pt.postid = p.postid
                inner join tags t on t.tagid = pt.tagid";

            var posts = await conn.QueryAsync<Post, Tag, Post>(query2, (post, tag) => {
                post.Tags.Add(tag);
                return post;}, splitOn: "tagid");

            var result = posts.GroupBy(p => p.PostId).Select(g =>
            {
                var groupedPost = g.First();
                groupedPost.Tags = g.Select(p => p.Tags.Single()).ToList();
                return groupedPost;
            });
            foreach (var post in result)
            {
                msg += $"{post.Headline}: ";
                foreach (var tag in post.Tags)
                {
                    msg += $" {tag.TagName} " + Environment.NewLine;
                }
            }
            return msg;
        }
//#nullable enable
        //return "unreachable code";
    }

    public int InsertStudentMarks(CreateStudentMark student)
    {
        using (var conn = new SqlConnection(_connectionStringHolder.Value))
        {
            //conn.Open(); //qq does this need to be opened? No.
            var affectedRows = conn.Execute("Insert into StudentMarks (Name, Subject, Marks) values (@Name, @subject, @Marks)", new { Name = student.Name, Subject = student.Subject, Marks = student.Marks });
            //conn.Close();
            return affectedRows;
        }
    }
    //Dapper methods; these are all extension methods of IDbConnection.
    //Execute
    //Query
    //QueryFirstOrDefault
    //QuerySingle
    //QuerySingleOrDefault
    //QueryMultiple

}
