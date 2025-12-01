using backSendify.Users.Application.Users.Dtos;

namespace backSendify.Users.Application.Auth.Dtos;

public record LoginResponseDto(UserDto User, string Token);
