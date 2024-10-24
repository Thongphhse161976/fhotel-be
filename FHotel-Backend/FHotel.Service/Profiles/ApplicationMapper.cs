﻿using AutoMapper;
using FHotel.Repository.Models;
using FHotel.Service.DTOs.Amenities;
using FHotel.Service.DTOs.BillLateCheckOutCharges;
using FHotel.Service.DTOs.BillOrders;
using FHotel.Service.DTOs.Bills;
using FHotel.Service.DTOs.Districts;
using FHotel.Service.DTOs.Facilities;
using FHotel.Service.DTOs.Hotels;
using FHotel.Service.DTOs.HotelStaffs;
using FHotel.Service.DTOs.LateCheckOutCharges;
using FHotel.Service.DTOs.LateCheckOutPolicies;
using FHotel.Service.DTOs.RefundPolicies;
using FHotel.Service.DTOs.Refunds;
using FHotel.Service.DTOs.Reservations;
using FHotel.Service.DTOs.Rooms;
using FHotel.Service.DTOs.RoomStayHistories;
using FHotel.Service.DTOs.RoomTypes;
using FHotel.Service.DTOs.Services;
using FHotel.Service.DTOs.TypePricings;
using FHotel.Service.DTOs.Types;
using FHotel.Service.DTOs.Users;
using FHotel.Service.DTOs.WalletHistories;
using FHotel.Service.DTOs.Wallets;
using FHotel.Services.DTOs.BillReservations;
using FHotel.Services.DTOs.Cities;
using FHotel.Services.DTOs.Countries;
using FHotel.Services.DTOs.Documents;
using FHotel.Services.DTOs.Feedbacks;
using FHotel.Services.DTOs.HotelAmenities;
using FHotel.Services.DTOs.HotelDocuments;
using FHotel.Services.DTOs.HotelImages;
using FHotel.Services.DTOs.Hotels;
using FHotel.Services.DTOs.OrderDetails;
using FHotel.Services.DTOs.Orders;
using FHotel.Services.DTOs.PaymentMethods;
using FHotel.Services.DTOs.Reservations;
using FHotel.Services.DTOs.RevenueSharePolicies;
using FHotel.Services.DTOs.Roles;
using FHotel.Services.DTOs.RoomFacilities;
using FHotel.Services.DTOs.RoomImages;
using FHotel.Services.DTOs.Rooms;
using FHotel.Services.DTOs.RoomTypes;
using FHotel.Services.DTOs.Services;
using FHotel.Services.DTOs.ServiceTypes;
using FHotel.Services.DTOs.UserDocuments;
using FHotel.Services.DTOs.Users;


namespace FHotel.Service.Profiles
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Amenity, AmenityRequest>().ReverseMap();
            CreateMap<Amenity, AmenityResponse>().ReverseMap();
            CreateMap<Role, RoleRequest>().ReverseMap();
            CreateMap<Role, RoleResponse>().ReverseMap();
            CreateMap<City, CityRequest>().ReverseMap();
            CreateMap<City, CityResponse>().ReverseMap();
            CreateMap<Country, CountryRequest>().ReverseMap();
            CreateMap<Country, CountryResponse>().ReverseMap();
            CreateMap<District, DistrictRequest>().ReverseMap();
            CreateMap<District, DistrictResponse>().ReverseMap();
            CreateMap<Document, DocumentRequest>().ReverseMap();
            CreateMap<Document, DocumentResponse>().ReverseMap();
            CreateMap<Facility, FacilityRequest>().ReverseMap();
            CreateMap<Facility, FacilityResponse>().ReverseMap();
            CreateMap<Feedback, FeedbackRequest>().ReverseMap();
            CreateMap<Feedback, FeedbackResponse>().ReverseMap();
            CreateMap<Hotel, HotelCreateRequest>().ReverseMap();
            CreateMap<Hotel, HotelRequest>().ReverseMap();
            CreateMap<Hotel, HotelUpdateRequest>().ReverseMap();
            CreateMap<Hotel, HotelResponse>().ReverseMap();
            CreateMap<HotelStaff, HotelStaffResponse>().ReverseMap();
            CreateMap<HotelStaff, HotelStaffCreateRequest>().ReverseMap();
            CreateMap<HotelAmenity, HotelAmenityRequest>().ReverseMap();
            CreateMap<HotelAmenity, HotelAmenityResponse>().ReverseMap();
            CreateMap<Order, OrderRequest>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailRequest>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailResponse>().ReverseMap();
            CreateMap<PaymentMethod, PaymentMethodRequest>().ReverseMap();
            CreateMap<PaymentMethod, PaymentMethodResponse>().ReverseMap();
            CreateMap<Reservation, ReservationCreateRequest>().ReverseMap();
            CreateMap<Reservation, ReservationUpdateRequest>().ReverseMap();
            CreateMap<Reservation, ReservationResponse>().ReverseMap();
            CreateMap<Room, RoomRequest>().ReverseMap();
            CreateMap<Room, RoomCreateRequest>().ReverseMap();
            CreateMap<Room, RoomUpdateRequest>().ReverseMap();
            CreateMap<Room, RoomResponse>().ReverseMap();
            CreateMap<RoomFacility, RoomFacilityRequest>().ReverseMap();
            CreateMap<RoomFacility, RoomFacilityResponse>().ReverseMap();
            CreateMap<RoomImage, RoomImageRequest>().ReverseMap();
            CreateMap<RoomImage, RoomImageResponse>().ReverseMap();
            CreateMap<RoomType, RoomTypeCreateRequest>().ReverseMap();
            CreateMap<RoomType, RoomTypeUpdateRequest>().ReverseMap();
            CreateMap<RoomType, RoomTypeResponse>().ReverseMap();
            CreateMap<Repository.Models.Service, ServiceCreateRequest>().ReverseMap();
            CreateMap<Repository.Models.Service, ServiceUpdateRequest>().ReverseMap();
            CreateMap<Repository.Models.Service, ServiceResponse>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeRequest>().ReverseMap();
            CreateMap<ServiceType, ServiceTypeResponse>().ReverseMap();
            CreateMap<Repository.Models.Type, TypeCreateRequest>().ReverseMap();
            CreateMap<Repository.Models.Type, TypeUpdateRequest>().ReverseMap();
            CreateMap<Repository.Models.Type, TypeResponse>().ReverseMap();
            CreateMap<TypePricing, TypePricingCreateRequest>().ReverseMap();
            CreateMap<TypePricing, TypePricingUpdateRequest>().ReverseMap();
            CreateMap<TypePricing, TypePricingResponse>().ReverseMap();
            CreateMap<User, UserRequest>().ReverseMap();
            CreateMap<User, UserCreateRequest>().ReverseMap();
            CreateMap<User, UserUpdateRequest>().ReverseMap();
            CreateMap<User, UserLoginRequest>().ReverseMap();
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<Bill, BillRequest>().ReverseMap();
            CreateMap<Bill, BillResponse>().ReverseMap();
            CreateMap<BillOrder, BillOrderRequest>().ReverseMap();
            CreateMap<BillOrder, BillOrderResponse>().ReverseMap();
            CreateMap<BillLateCheckOutCharge, BillLateCheckOutChargeRequest>().ReverseMap();
            CreateMap<BillLateCheckOutCharge, BillLateCheckOutChargeResponse>().ReverseMap();
            CreateMap<BillReservation, BillReservationRequest>().ReverseMap();
            CreateMap<BillReservation, BillReservationResponse>().ReverseMap();
            CreateMap<LateCheckOutCharge, LateCheckOutChargeRequest>().ReverseMap();
            CreateMap<LateCheckOutCharge, LateCheckOutChargeResponse>().ReverseMap();
            CreateMap<LateCheckOutPolicy, LateCheckOutPolicyRequest>().ReverseMap();
            CreateMap<LateCheckOutPolicy, LateCheckOutPolicyResponse>().ReverseMap();
            CreateMap<Refund, RefundRequest>().ReverseMap();
            CreateMap<Refund, RefundResponse>().ReverseMap();
            CreateMap<RefundPolicy, RefundPolicyRequest>().ReverseMap();
            CreateMap<RefundPolicy, RefundPolicyResponse>().ReverseMap();
            CreateMap<RoomStayHistory, RoomStayHistoryRequest>().ReverseMap();
            CreateMap<RoomStayHistory, RoomStayHistoryResponse>().ReverseMap();
            CreateMap<Wallet, WalletRequest>().ReverseMap();
            CreateMap<Wallet, WalletResponse>().ReverseMap();
            CreateMap<WalletHistory, WalletHistoryRequest>().ReverseMap();
            CreateMap<WalletHistory, WalletHistoryResponse>().ReverseMap();
            CreateMap<HotelAmenity, HotelAmenityResponse>().ReverseMap();
            //new
            CreateMap<HotelImage, HotelImageRequest>().ReverseMap();
            CreateMap<HotelImage, HotelImageResponse>().ReverseMap();
            CreateMap<HotelDocument, HotelDocumentRequest>().ReverseMap();
            CreateMap<HotelDocument, HotelDocumentResponse>().ReverseMap();
            CreateMap<UserDocument, UserDocumentRequest>().ReverseMap();
            CreateMap<UserDocument, UserDocumentResponse>().ReverseMap();
            CreateMap<RevenueSharePolicy, RevenueSharePolicyRequest>().ReverseMap();
            CreateMap<RevenueSharePolicy, RevenueSharePolicyResponse>().ReverseMap();

        }
          
    }
}
