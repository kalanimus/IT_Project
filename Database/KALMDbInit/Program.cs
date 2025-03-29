using ApplicationContext;

using (var context = new AppDbContext()){
  context.Database.EnsureCreated();
  Console.WriteLine("База данных успешно инициализирована без данных.");
}