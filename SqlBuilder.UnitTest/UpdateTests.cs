﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;

namespace SqlBuilder.UnitTest
{
  [TestClass]
  public class UpdateTests
  {
    [TestMethod]
    public void update_sql()
    {
      TestModel model = new TestModel
      {
        Title = "foo"
      };
      Update<TestModel> update = new Update<TestModel>(4, model);

      update.Sql().MustEqual("update dbo.TestModel set Title=@Title where Id=@Id");
      update.Parameters.Count.MustEqual(2);
      update.Parameters["Id"].MustEqual(4);
      update.Parameters["Title"].MustEqual("foo");
    }

    [TestMethod]
    public void update_includes_diff_model_params()
    {
      TestModel current = new TestModel { Title = "foo" };
      TestModel next = new TestModel { Title = "bar" };

      Update<TestModel> update = new Update<TestModel>(4, current, next);

      update.Parameters.Count.MustEqual(2);
      update.Parameters["Title"].MustEqual("bar");
      update.Parameters["Id"].MustEqual(4);
    }

    [TestMethod]
    public void update_does_not_include_diff_model_params()
    {
      TestModel current = new TestModel { Title = "foo" };
      TestModel next = new TestModel { Title = "foo" };

      Update<TestModel> update = new Update<TestModel>(4, current, next);

      update.Parameters.Count.MustEqual(0);
      update.Sql().MustBeNull();
    }

    [TestMethod]
    public void update_ctor_fails_when_model_does_not_have_a_key()
    {
      Action action = () =>
      {
        TestModelNoKey model = new TestModelNoKey();
        Update<TestModelNoKey> update = new Update<TestModelNoKey>(4, model);
      };

      action.MustThrow<InvalidOperationException>();
    }

    private class TestModel
    {
      [Key]
      public int Id { get; set; }

      public string Title { get; set; }
    }

    private class TestModelNoKey
    {
      public int Id { get; set; }
    }
  }
}