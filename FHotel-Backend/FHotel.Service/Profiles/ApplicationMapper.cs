using AutoMapper;
using FHotel.Repository.Models;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.DamagedFactilities;
using FHotel.Services.DTOs.Documents;
using FHotel.Services.DTOs.Feedbacks;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Services.DTOs.HotelRegistations;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.PaymentMethods;
using FHotel.Services.DTOs.Payments;
using FHotel.Services.DTOs.Prices;
using FHotel.Services.DTOs.ReservationDetails;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.DTOs.Roles;
using FHotel.Services.DTOs.RoomFacilities;
using FHotel.Services.DTOs.RoomImages;
using FHotel.Services.DTOs.Rooms;
using FHotel.Services.DTOs.RoomTypePrices;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.DTOs.Services;
using FHotel.Services.DTOs.ServiceTypes;
using FHotel.Services.DTOs.Timetable;
using FHotel.Services.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHotel.Service.Profiles
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Role, RoleRequest>().ReverseMap();
            CreateMap<Role, RoleResponse>().ReverseMap();
            CreateMap<City, CityRequest>().ReverseMap();
            CreateMap<City, CityResponse>().ReverseMap();
            CreateMap<Country, CountryRequest>().ReverseMap();
            CreateMap<Country, CountryResponse>().ReverseMap();
            CreateMap<DamagedFacility, DamagedFacilityRequest>().ReverseMap();
            CreateMap<DamagedFacility, DamagedFacilityResponse>().ReverseMap();
            CreateMap<Document, DocumentRequest>().ReverseMap();
            CreateMap<Document, DocumentResponse>().ReverseMap();
            CreateMap<Feedback, FeedbackRequest>().ReverseMap();
            CreateMap<Feedback, FeedbackResponse>().ReverseMap();
            CreateMap<Hotel, HotelRequest>().ReverseMap();
            CreateMap<Hotel, HotelResponse>().ReverseMap();
            CreateMap<HotelAmenity, HotelAmenityRequest>().ReverseMap();
            CreateMap<HotelAmenity, HotelAmenityResponse>().ReverseMap();
            CreateMap<HotelRegistration, HotelRegistrationRequest>().ReverseMap();
            CreateMap<HotelRegistration, HotelRegistrationResponse>().ReverseMap();
            CreateMap<Order, OrderRequest>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailRequest>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailResponse>().ReverseMap();
            CreateMap<Payment, PaymentRequest>().ReverseMap();
            CreateMap<Payment, PaymentResponse>().ReverseMap();
            CreateMap<PaymentMethod, PaymentMethodRequest>().ReverseMap();
            CreateMap<PaymentMethod, PaymentMethodResponse>().ReverseMap();
            CreateMap<Price, PriceRequest>().ReverseMap();
            CreateMap<Price, PriceResponse>().ReverseMap();
            CreateMap<Reservation, ReservationRequest>().ReverseMap();
            CreateMap<Reservation, ReservationResponse>().ReverseMap();
            CreateMap<ReservationDetail, ReservationDetailRequest>().ReverseMap();
            CreateMap<ReservationDetail, ReservationDetailResponse>().ReverseMap();
            CreateMap<Room, RoomRequest>().ReverseMap();
            CreateMap<Room, RoomResponse>().ReverseMap();
            CreateMap<RoomFacility, RoomFacilityRequest>().ReverseMap();
            CreateMap<RoomFacility, RoomFacilityResponse>().ReverseMap();
            CreateMap<RoomImage, RoomImageRequest>().ReverseMap();
            CreateMap<RoomImage, RoomImageResponse>().ReverseMap();
            CreateMap<RoomType, RoomTypeRequest>().ReverseMap();
            CreateMap<RoomType, RoomTypeResponse>().ReverseMap();
            CreateMap<RoomTypePrice, RoomTypePriceRequest>().ReverseMap();
            CreateMap<RoomTypePrice, RoomTypePriceResponse>().ReverseMap();
            CreateMap<Repository.Models.Service, ServiceRequest>().ReverseMap();
            CreateMap<Repository.Models.Service, ServiceResponse>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeRequest>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeResponse>().ReverseMap();
            CreateMap<Timetable, TimetableRequest>().ReverseMap();
            CreateMap<Timetable, TimetableResponse>().ReverseMap();
            CreateMap<User, UserRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();

        }
          
    }
}
