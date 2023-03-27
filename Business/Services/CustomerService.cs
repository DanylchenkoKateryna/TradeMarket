using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            this._unitOfWork = _unitOfWork;
            this._mapper = _mapper;
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var allCustomers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CustomerModel>>(allCustomers);
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var obj = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<CustomerModel>(obj);
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var allCustomers = await _unitOfWork.CustomerRepository.GetAllWithDetailsAsync();
            var result = allCustomers.Where(x => x.Receipts.Any(x => x.ReceiptDetails.Any(x => x.ProductId == productId)));
            return _mapper.Map<IEnumerable<CustomerModel>>(result);
        }

        public async Task AddAsync(CustomerModel model)
        {
            _unitOfWork.CustomerRepository.Add(_mapper.Map<Customer>(model));
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(int id,CustomerModel model)
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(id);
            customer.DiscountValue = model.DiscountValue;
            customer.Person.Name = model.Name;
            customer.Person.BirthDate = model.BirthDate;
            customer.Person.Surname = model.Surname;
            _unitOfWork.CustomerRepository.Update(customer);
        }

        public async Task DeleteAsync(int modelId)
        {
            var toBeDeleted = await _unitOfWork.CustomerRepository.GetByIdWithDetailsAsync(modelId);
            _unitOfWork.PersonRepository.Delete(toBeDeleted.Person);
            _unitOfWork.CustomerRepository.Delete(toBeDeleted);
            await _unitOfWork.SaveAsync();
        }

        public CustomerModel mappToCustomerModel(CreateCustomerModel model)
        {
            return _mapper.Map<CustomerModel>(model);
        }
        public Customer mappToCustomer(CreateCustomerModel model)
        {
            var customerModel = mappToCustomerModel(model);
            return _mapper.Map<Customer>(customerModel);
        }
    }
}
